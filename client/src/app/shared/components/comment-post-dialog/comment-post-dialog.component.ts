import { Component, ChangeDetectorRef, NgZone, INJECTOR, inject, ViewEncapsulation, ChangeDetectionStrategy, ElementRef, ViewChild, Output, EventEmitter, Renderer2 } from '@angular/core';
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
import { Comment } from '../../../core/models/comment/comment.model';
import { BehaviorSubject } from 'rxjs';
import { CommentStateService } from '../../../core/services/state/comment-state/comment-state.service';
import e from 'cors';

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
  public readonly context = injectContext<TuiDialogContext<any>>();


  data: any;
  post: FeedPost;

  currentIndex: number = 0;
  currentImage: MediaModel | null;
  showFileInput: boolean = false;
  uploadedFile: File | null = null;

  form!: FormGroup;
  formData: FormData = new FormData();

  user: UserResponseModel;
  comments: Comment[] = [];

  isLoading: boolean = false;

  typeCloudinary = 'cloudinary';
  typeLocal = 'local';

  pendingComments: Comment[] = [];
  buttonClose: any;

  @ViewChild('commentContainer') commentContainer!: ElementRef;

  constructor(
    private formBuilder: RxFormBuilder,
    private _commentService: CommentService,
    private alertService: AlertService,
    private sanitizer: DomSanitizer,
    private _commentStateService: CommentStateService,
    private renderer: Renderer2,

  ) {
    this.data = this.context.data;
    this.user = Helper.getUserFromLocalStorage();
    this.form = this.formBuilder.formGroup(CommentRequestModel, {});

    document.getElementsByClassName('t-item ng-star-inserted')
  }

  ngOnInit() {
    this.post = this.data.post;
    this.getCommentsPost();
    this.handleButtonClose();
  }



  handleButtonClose() {
    const buttonCloseCollection = document.getElementsByClassName('t-close');
  
    if (buttonCloseCollection.length > 0) {
      Array.from(buttonCloseCollection).forEach((buttonClose) => {
        const buttonElement = buttonClose as HTMLElement;
  
        buttonElement.addEventListener('click', () => {
          console.log('Button clicked');
          console.log('Context:', this.context);
          console.log('Comments:', this.comments);
  
          // Ensure context and comments are valid
          if (this.context && this.comments) {
            this.context.completeWith(this.comments);
          } else {
            console.error('Unable to complete dialog: Context or comments are invalid');
          }
        });
      });
    } else {
      console.log("No elements found with the class 't-close'");
    }
  }
  
  
  
  handleMedia() {
    if (this.post?.media && this.post.media.length > 0) {
      this.post.media.map((media) => {
        this.handleUrlLargeMedia(media.url);
      });
    }
  }


  sortComments(a: any, b: any): number {
    const dateA = new Date(a.createdAt).getTime();
    const dateB = new Date(b.createdAt).getTime();
    return dateB - dateA;
  }



  getCommentsPost() {
    this.isLoading = true;
    this._commentService.getCommentsByPostId(this.post.id).subscribe({
      next: (res) => {
        if (res) {
          this.comments = res.result;
          this.comments = this.comments?.sort(this.sortComments);
          this._commentStateService.setComments(this.comments);
          this.isLoading = false;
        }
      },
      error: (error) => {
        console.log(error);
        this.isLoading = false;
      },
      complete: () => {
        this.isLoading = false;
      }
    });
  }


  ngAfterViewChecked(): void {
    // Automatically scroll to bottom whenever the view is checked
    this.scrollToBottom();
  }

  ngOnChanges() {

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
            this.comments.push(res.result);
            this._commentStateService.setComments(this.comments);
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

  enlargeMedia(mediaLarge: any, type: string) {
    if (type === 'cloudinary') {
      const media = mediaLarge as MediaModel;
      this.dialogs
        .open(
          new PolymorpheusComponent(EnlargeImageComponentComponent, this.injector),
          {
            data: { media, type },
            size: 'auto',
            appearance: 'lorem-ipsum',
          }
        )
        .subscribe((data) => {
          console.log(data);
        });
    }
    else {
      let media: MediaModel = {
        publicId: '',
        url: '',
        contentType: mediaLarge.type.split('/')[0],
        thumbnailUrl: '',
        duration: 0,
        width: 0,
        height: 0,
        fileSize: 0,
        format: '',
      }
      const fileUrl = URL.createObjectURL(mediaLarge);
      media.url = fileUrl as any;
      this.dialogs
        .open(
          new PolymorpheusComponent(EnlargeImageComponentComponent, this.injector),
          {
            data: { media, type },
            size: 'auto',
            appearance: 'lorem-ipsum',

          }
        )
        .subscribe((data) => {
          console.log(data);
        });
    }

  }

  handleUrlLargeMedia(url: string): string {
    return url.replace('/upload/', '/upload/w_1000/');
  }
}