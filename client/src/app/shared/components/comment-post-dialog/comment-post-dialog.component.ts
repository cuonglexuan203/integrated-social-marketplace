import { Component } from '@angular/core';
import { TuiButton, TuiDialogContext, TuiIcon, TuiLink, TuiLoader } from '@taiga-ui/core';
import { injectContext } from '@taiga-ui/polymorpheus';
import { FeedPost } from '../../../core/models/feed/feed.model';
import { CommonModule } from '@angular/common';
import { TuiAvatar, TuiCarousel, TuiFiles, TuiPagination, TuiSkeleton } from '@taiga-ui/kit';
import { FirstLetterWordPipe } from '../../../core/pipes/first-letter-word/first-letter-word.pipe';
import { MediaModel } from '../../../core/models/media/media.model';
import { FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RxFormBuilder, RxReactiveFormsModule } from '@rxweb/reactive-form-validators';
import { TuiTextfieldControllerModule } from '@taiga-ui/legacy';
import { CommentRequestModel } from '../../../core/models/comment/comment-request.model';
import { FeedService } from '../../../core/services/feed/feed.service';
import { UserResponseModel } from '../../../core/models/user/user.model';
import { Helper } from '../../../core/utils/helper';
import { AlertService } from '../../../core/services/alert/alert.service';
import { CommentService } from '../../../core/services/comment/comment.service';


@Component({
  selector: 'app-comment-post-dialog',
  standalone: true,
  imports: [
    CommonModule,
    TuiCarousel,
    TuiButton,
    TuiPagination,
    TuiAvatar,
    FirstLetterWordPipe,
    TuiIcon,
    RxReactiveFormsModule,
    FormsModule,
    TuiFiles,
    TuiLink,
    TuiTextfieldControllerModule,
    ReactiveFormsModule,
    TuiSkeleton,
    TuiLoader,
  ],
  templateUrl: './comment-post-dialog.component.html',
  styleUrl: './comment-post-dialog.component.css'
})
export class CommentPostDialogComponent {
  data: any;
  post: FeedPost;
  
  currentIndex: number = 0;
  currentImage: MediaModel | null;
  showFileInput: boolean = false;
  
  form!: FormGroup;
  formData: FormData = new FormData();
  
  user: UserResponseModel;

  isLoading: boolean = false;
  public readonly context = injectContext<TuiDialogContext<any>>();

  constructor(
    private formBuilder: RxFormBuilder,
    private _commentService: CommentService,
    private alertService: AlertService
  ) {
    this.data = this.context.data;
    this.user = Helper.getUserFromLocalStorage();
    this.form = this.formBuilder.formGroup(CommentRequestModel, {});
  }

  sortComments(a: any, b: any) {
    return new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime();
  }

  ngOnInit() {
    this.post = this.data.post;
    this.post.comments = this.post.comments.sort(this.sortComments);
    this.post.comments.map((comment: any) => {
      console.log(comment.media[0]);
    });  
    if (this.post?.media?.length > 0) {
      this.currentImage = this.post.media[0];
    }
  }


  onCarouselChange(index: number) {
    this.currentIndex = index;
    if (this.post?.media && this.post.media[index]) {
      this.currentImage = this.post.media[index];
    }
  }


  autoResize(event: any): void {
    const textarea = event.target;
    textarea.style.height = 'auto';
    textarea.style.height = textarea.scrollHeight + 'px';

    // Optional: Set maximum height
    const maxHeight = 100;
    if (textarea.scrollHeight > maxHeight) {
      textarea.style.height = maxHeight + 'px';
      textarea.style.overflowY = 'auto';
    } else {
      textarea.style.overflowY = 'hidden';
    }
  }

  toggleFileInput() {
    this.showFileInput = !this.showFileInput;

  }

  removeFile() {
    this.form.get('media')?.setValue(null);
  }

  setupDataForComment() {
    this.form.get('postId')?.setValue(this.post.id);
    this.form.get('userId')?.setValue(this.user.id);

    this.formData.append('postId', this.post.id);
    this.formData.append('userId', this.user.id);
    this.formData.append('commentText', this.form.get('commentText')?.value);

    const mediaControl = this.form.get('media');
    if (mediaControl?.value) {
      this.formData.append('media', mediaControl.value);
    }
  }

  resetData () {
    this.formData = new FormData();
    this.form.reset();
    this.showFileInput = false;
  }

  onSubmit(event: any) {
    event.preventDefault();
    this.setupDataForComment()
    this.isLoading = true;
    if (this.form.valid) {
      this._commentService.createComment(this.formData).subscribe({
        next: (res) => {
          if (res) {
            this.alertService.showSuccess('Comment', 'Comment created successfully');
            this.resetData();
            this.post.comments.push(res?.result);
            this.isLoading = false;
          }
          else {
            this.isLoading = false;
          }
        },
        error: (error) => {
          console.log(error);
          this.isLoading = false;
          this.alertService.showError('Comment', 'Comment creation failed');
        },
        complete: () => {
          this.resetData();
          this.isLoading = false;
        }
      })
    }
  }


}
