import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, ElementRef, EventEmitter, inject, INJECTOR, ViewChild } from '@angular/core';
import { FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RxFormBuilder, RxReactiveFormsModule } from '@rxweb/reactive-form-validators';
import { TuiButton, TuiDialogContext, TuiDialogService, TuiIcon, TuiLink } from '@taiga-ui/core';
import { TuiAvatar, TuiFiles, TuiStepper } from '@taiga-ui/kit';
import { injectContext, PolymorpheusComponent } from '@taiga-ui/polymorpheus';
import { CreatePostModel } from '../../../core/models/feed/post.model';
import { Helper } from '../../../core/utils/helper';
import { AlertService } from '../../../core/services/alert/alert.service';
import { TuiInputTagModule, TuiTextfieldControllerModule } from '@taiga-ui/legacy';
import { FeedService } from '../../../core/services/feed/feed.service';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { EnlargeImageComponentComponent } from '../enlarge-image-component/enlarge-image-component.component';
import { MediaModel } from '../../../core/models/media/media.model';
import { ConfirmationDialogComponent } from '../confirmation-dialog/confirmation-dialog.component';

@Component({
  selector: 'app-create-post-dialog',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    RxReactiveFormsModule,
    TuiButton,
    TuiIcon,
    CommonModule,
    TuiAvatar,
    TuiFiles,
    TuiLink,
    TuiInputTagModule,
    TuiTextfieldControllerModule
  ],
  templateUrl: './create-post-dialog.component.html',
  styleUrl: './create-post-dialog.component.css'
})
export class CreatePostDialogComponent {
  @ViewChild('textArea') textArea: ElementRef;

  isLoading: boolean = false;

  form!: FormGroup;
  user: any;

  data: any;
  title: string = '';

  currentTime: Date = new Date();
  timeUpdateInterval: any;

  showFileInput = false;

  uploadedFile: File[] = [];
  formData: FormData = new FormData();

  private readonly injector = inject(INJECTOR);
  private readonly dialogs = inject(TuiDialogService);

  public readonly context = injectContext<TuiDialogContext<any>>();

  constructor(
    private formBuilder: RxFormBuilder,
    private cdr: ChangeDetectorRef,
    private alertService: AlertService,
    private _feedService: FeedService,
    private sanitizer: DomSanitizer
  ) {
    this.data = this.context.data;
    this.form = this.formBuilder.formGroup(CreatePostModel, {});
  }
  ngOnInit() {
    this.title = this.data.title;
    this.getUser();
    this.startTimeUpdate();
  }

  ngOnDestroy() {
    if (this.timeUpdateInterval) {
      clearInterval(this.timeUpdateInterval);
    }

    this.data = null;
    // this.checkDataHasChanged();
  }

  // checkDataHasChanged() {
  //   if (this.form?.dirty) {
  //     this.dialogs.open(
  //       new PolymorpheusComponent(ConfirmationDialogComponent, this.injector), {
  //       data: {
  //         title: 'Are you sure you want to leave?',
  //         description: 'Your changes will be lost.'
  //       },
  //       size: 'auto',
  //       appearance: 'lorem-ipsum',
  //     }).subscribe((data) => {
  //       this.context.completeWith(data);
  //     });
  //   }
  // }

  ngAfterContentChecked() {

    this.cdr.detectChanges();
  }

  getUser() {
    this.user = Helper.getUserFromLocalStorage();
    this.form.get('userId')?.setValue(this.user?.id);

  }

  getCurrentTime() {
    this.currentTime = new Date(Helper.getCurrentTime());
  }

  handleMedia(media: MediaModel) {
    media.url = this.handleUrlLargeMedia(media.url);
  }

  handleUrlLargeMedia(url: string): string {
    return url.replace('/upload/', '/upload/w_1000/');
  }

  startTimeUpdate() {
    // Update time every second
    this.timeUpdateInterval = setInterval(() => {
      this.getCurrentTime();
    }, 60000);
  }


  get f() {
    return this.form.controls;
  }



  toggleFileInput() {
    this.showFileInput = !this.showFileInput;
  }

  onFileUpload(event: Event) {
    const files = (event.target as HTMLInputElement).files;
    if (files) {
      for (let i = 0; i < files.length; i++) {
        const allowedImageExtensions = ['.png', '.jpg', '.jpeg', '.gif'];
        const allowedVideoExtensions = ['.mp4', '.mpeg', '.quicktime'];
        if (allowedImageExtensions.includes(files[i].name.substring(files[i].name.lastIndexOf('.'))) || allowedVideoExtensions.includes(files[i].name.substring(files[i].name.lastIndexOf('.')))) {
          if (files[i].size < 10000000) {
            this.uploadedFile.push(files[i]);
          }
          else {
            this.alertService.showError('File size is too large', 'Error');
          }
        }
        else {
          this.alertService.showError('Only image and video files are allowed', 'Error');
        }
      }
      this.form.get('files')?.setValue(this.uploadedFile);
    }
    console.log(this.uploadedFile);

  }

  removeFile(file: File) {
    this.uploadedFile = this.uploadedFile.filter(f => f !== file);
  }

  setupDataForPost() {
    this.uploadedFile.forEach(file => {
      this.formData.append('files', file);
    });
    this.formData.append('userId', this.user?.id);
    this.formData.append('contentText', this.form.get('contentText')?.value);

    let tags = this.form.get('tags')?.value;

    if (tags) {
      tags.forEach((tag: string) => {
        this.formData.append('tags', tag);
      });
    }

  }


  onPost() {
    this.setupDataForPost();
    this.isLoading = true;
    if (this.form.valid) {
      this._feedService.createPost(this.formData).subscribe({
        next: (res) => {
          if (res) {
            this.isLoading = false;
            this.alertService.showSuccess('Create a new post successfully', 'Success');
            this.context.completeWith(res?.result);
          }
        },
        error: (error) => {
          this.isLoading = false;
          this.alertService.showError('Cannot create a new post', 'Error');
        },
        complete: () => {
          this.isLoading = false;
        }
      })
    }
  }

  getFileUrl(file: File): SafeUrl | null {
    if (!file) return null;

    const objectUrl = URL.createObjectURL(file);
    return this.sanitizer.bypassSecurityTrustUrl(objectUrl);
  }

  isImage(file: File): boolean {
    const allowedImageExtensions = ['.png', '.jpg', '.jpeg', '.gif'];
    const fileExtension = file.name.substring(file.name.lastIndexOf('.')).toLowerCase();
    return allowedImageExtensions.includes(fileExtension);
  }

  isVideo(file: File): boolean {
    const allowedVideoExtensions = ['.mp4', '.mpeg', '.quicktime'];
    const fileExtension = file.name.substring(file.name.lastIndexOf('.')).toLowerCase();
    return allowedVideoExtensions.includes(fileExtension);
  }


  enlargeMedia(file: File) {
    console.log(file, "before");

    let media: MediaModel = {
      publicId: '',
      url: '',
      contentType: file.type.split('/')[0],
      thumbnailUrl: '',
      duration: 0,
      width: 0,
      height: 0,
      fileSize: 0,
      format: '',
    }
    const fileUrl = URL.createObjectURL(file);
    media.url = fileUrl as any;
    const type = 'local';
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
