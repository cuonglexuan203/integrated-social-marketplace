import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component } from '@angular/core';
import { FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RxFormBuilder, RxReactiveFormsModule } from '@rxweb/reactive-form-validators';
import { TuiButton, TuiDialogContext, TuiIcon } from '@taiga-ui/core';
import { TuiStepper } from '@taiga-ui/kit';
import { injectContext } from '@taiga-ui/polymorpheus';
import { CreatePostModel } from '../../../core/models/feed/post.model';

@Component({
  selector: 'app-create-post-dialog',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    RxReactiveFormsModule,
    TuiButton,
    TuiIcon,
    TuiStepper,
    CommonModule,
  ],
  templateUrl: './create-post-dialog.component.html',
  styleUrl: './create-post-dialog.component.css'
})
export class CreatePostDialogComponent {
  form!: FormGroup;
  public readonly context = injectContext<TuiDialogContext<any>>();
  data: any;
  title: string = '';
  steps: any[] = [
    {
      steps: 'Step 1',
      title: 'Enter Post Details, Tags',
      isPassed: false,
      template: 'step1',
    },
    {
      steps: 'Step 2',
      title: 'Upload Images, Videos',
      isPassed: false,
      template: 'step2',
    }
  ];
  activeTabIndex = 0;
  constructor(
    private formBuilder: RxFormBuilder,
    private cdr: ChangeDetectorRef,
  ) {
    this.data = this.context.data;
    this.form = this.formBuilder.formGroup(CreatePostModel, {});
  }

  ngAfterContentChecked() {
    this.cdr.detectChanges();
  }

  ngOnInit() {
    this.title = this.data.title;
  }

  get f() {
    return this.form.controls;
  }


  onSubmit() {
    
  }

  onNextStep() {
    this.activeTabIndex++;
  }

}
