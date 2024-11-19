import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, ElementRef, ViewChild } from '@angular/core';
import { FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RxFormBuilder, RxReactiveFormsModule } from '@rxweb/reactive-form-validators';
import { TuiButton, TuiDialogContext, TuiIcon, TuiLink } from '@taiga-ui/core';
import { TuiAvatar, TuiFiles, TuiStepper } from '@taiga-ui/kit';
import { injectContext } from '@taiga-ui/polymorpheus';
import { CreatePostModel } from '../../../core/models/feed/post.model';
import { Helper } from '../../../core/utils/helper';
import { AlertService } from '../../../core/services/alert/alert.service';
import { TuiInputTagModule, TuiTextfieldControllerModule } from '@taiga-ui/legacy';
import { FeedService } from '../../../core/services/feed/feed.service';

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
  showFileInput = false;
  timeUpdateInterval: any;
  uploadedFile: File[] = [];
  formData: FormData = new FormData();

  public readonly context = injectContext<TuiDialogContext<any>>();

  constructor(
    private formBuilder: RxFormBuilder,
    private cdr: ChangeDetectorRef,
    private alertService: AlertService,
    private _feedService: FeedService
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
  }

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
          if(files[i].size < 10000000) {
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
      this.formData.append('tags', this.form.get('tags')?.value);
  }

  onPost() {
    this.setupDataForPost();
    this.isLoading = true;
    if (this.form.valid) {
      this._feedService.createPost(this.formData).subscribe({
        next: (res) => {
          if (res){
            this.isLoading = false;
            this.alertService.showSuccess('Create a new post successfully', 'Success');
            this.context.completeWith(this.form.value);
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

}
