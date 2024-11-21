import { Component, ChangeDetectorRef, NgZone, INJECTOR, inject, ViewEncapsulation, ChangeDetectionStrategy, ElementRef, ViewChild } from '@angular/core';
import { TuiButton, TuiDialogContext, TuiDialogService, TuiIcon, TuiLink, TuiLoader } from '@taiga-ui/core';
import { injectContext, PolymorpheusComponent } from '@taiga-ui/polymorpheus';
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
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { EnlargeImageComponentComponent } from '../enlarge-image-component/enlarge-image-component.component';

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
  styleUrls: ['./comment-post-dialog.component.css'],

})
export class CommentPostDialogComponent {
  private readonly injector = inject(INJECTOR);
  private readonly dialogs = inject(TuiDialogService);

  data: any;
  post: FeedPost;

  currentIndex: number = 0;
  currentImage: MediaModel | null;
  showFileInput: boolean = false;
  uploadedFile: File | null = null;

  form!: FormGroup;
  formData: FormData = new FormData();

  user: UserResponseModel;

  isLoading: boolean = false;
  public readonly context = injectContext<TuiDialogContext<any>>();

  @ViewChild('commentContainer') commentContainer!: ElementRef;

  constructor(
    private formBuilder: RxFormBuilder,
    private _commentService: CommentService,
    private alertService: AlertService,
    private sanitizer: DomSanitizer,

  ) {
    this.data = this.context.data;
    this.user = Helper.getUserFromLocalStorage();
    this.form = this.formBuilder.formGroup(CommentRequestModel, {});
  }

  sortComments(a: any, b: any): number {
    const dateA = new Date(a.createdAt).getTime();
    const dateB = new Date(b.createdAt).getTime();
    return dateB - dateA;
  }

  ngOnInit() {
    this.post = this.data.post;
    this.post.comments = this.post.comments.sort(this.sortComments);
  }


  ngAfterViewChecked(): void {
    // Automatically scroll to bottom whenever the view is checked
    this.scrollToBottom();
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
    this.uploadedFile = null;
    this.form.get('media')?.setValue(null);
  }

  setupDataForComment() {
    this.form.get('postId')?.setValue(this.post.id);
    this.form.get('userId')?.setValue(this.user.id);

    this.formData.append('postId', this.post.id);
    this.formData.append('userId', this.user.id);
    if (this.form.get('commentText')?.value) {
      this.formData.append('commentText', this.form.get('commentText')?.value);
    }
    const mediaControl = this.form.get('media');
    if (mediaControl?.value) {
      this.formData.append('media', mediaControl.value);
    }
  }

  resetData() {
    this.formData = new FormData();
    this.form.reset();
    this.uploadedFile = null;
    this.showFileInput = false;
  }

  onSubmit(event: any) {
    event.preventDefault();
    this.setupDataForComment();
    this.isLoading = true;
    if (this.form.valid) {
      this._commentService.createComment(this.formData).subscribe({
        next: (res) => {
          if (res) {
            this.alertService.showSuccess('Comment', 'Comment created successfully');
            this.post.comments.push(res?.result);
            this.scrollToBottom();
            this.isLoading = false;
          } else {
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
            this.scrollToBottom();
        }
      });
    }
  }

  scrollToBottom(): void {
    if (this.commentContainer) {
      const element = this.commentContainer.nativeElement;
      element.scrollTop = element.scrollHeight;
    }
  }

  onFileUpload(event: Event): void {
      const files = (event.target as HTMLInputElement).files;
      if (files && files.length > 0) {
        const file = files[0];
        const allowedImageExtensions = ['.png', '.jpg', '.jpeg', '.gif'];
        const allowedVideoExtensions = ['.mp4', '.avi', '.flv', '.mov', '.wmv'];
        const fileExtension = file.name.substring(file.name.lastIndexOf('.')).toLowerCase();
        if (allowedImageExtensions.includes(fileExtension) || allowedVideoExtensions.includes(fileExtension)) {
          if (file.size < 10000000) {
            this.uploadedFile = file;
            this.form.get('media')?.setValue(file);
          } else {
            this.alertService.showError('File size is too large', 'Error');
          }
        } else {
          this.alertService.showError('Only image and video files are allowed', 'Error');
        }
      }
  }

  isImage(file: File): boolean {
    const allowedImageExtensions = ['.png', '.jpg', '.jpeg', '.gif'];
    const fileExtension = file.name.substring(file.name.lastIndexOf('.')).toLowerCase();
    return allowedImageExtensions.includes(fileExtension);
  }

  isVideo(file: File): boolean {
    const allowedVideoExtensions = ['.mp4', '.avi', '.flv', '.mov', '.wmv'];
    const fileExtension = file.name.substring(file.name.lastIndexOf('.')).toLowerCase();
    return allowedVideoExtensions.includes(fileExtension);
  }

  getFileUrl(file: File): SafeUrl | null {
    if (!file) return null;
    
    const objectUrl = URL.createObjectURL(file);
    return this.sanitizer.bypassSecurityTrustUrl(objectUrl);
  }

  enlargeMedia(media: MediaModel) {
    this.dialogs
    .open(
      new PolymorpheusComponent(EnlargeImageComponentComponent, this.injector),
      {
        data: media,
        size: 'auto',
        appearance: 'lorem-ipsum',
        
      }
    )
    .subscribe((data) => {
      console.log(data);
    });
  }
}