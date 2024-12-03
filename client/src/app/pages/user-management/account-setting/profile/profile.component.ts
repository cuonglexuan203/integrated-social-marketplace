import { CommonModule, formatDate } from '@angular/common';
import { Component } from '@angular/core';
import { FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TuiButton, TuiDataList, TuiIcon, TuiLoader, TuiNotification } from '@taiga-ui/core';
import {
  TuiAvatar, TuiDataListWrapper,
  TuiSkeleton,
} from '@taiga-ui/kit';
import { TuiComboBoxModule, TuiInputDateModule, TuiInputDateTimeModule, TuiInputModule, TuiInputTagModule, TuiSelectModule, TuiTextareaModule, TuiTextfieldControllerModule } from '@taiga-ui/legacy';
import { UserResponseModel } from '../../../../core/models/user/user.model';
import { Helper } from '../../../../core/utils/helper';
import { RxFormBuilder, RxReactiveFormsModule } from '@rxweb/reactive-form-validators';
import { UserDetailsModel } from '../../../../core/models/user/user-details.model';
import { UserService } from '../../../../core/services/user/user.service';
import { AlertService } from '../../../../core/services/alert/alert.service';
import {TuiDay, TuiTime} from '@taiga-ui/cdk';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    TuiAvatar,
    CommonModule,
    TuiButton,
    TuiInputModule,
    TuiTextareaModule,
    FormsModule,
    TuiSelectModule,
    TuiDataList,
    TuiDataListWrapper,
    TuiInputDateModule,
    FormsModule,
    ReactiveFormsModule,
    RxReactiveFormsModule,
    TuiIcon,
    TuiTextfieldControllerModule,
    TuiInputTagModule,
    TuiComboBoxModule,
    TuiSkeleton,
    TuiInputDateTimeModule,
    TuiNotification,
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent {
  isLoading: boolean = false;
  showWarning: boolean = true;
  form!: FormGroup;
  user: UserDetailsModel;

  genders: string[] = [
    'Male',
    'Female'
  ]
  constructor(
    private formBuilder: RxFormBuilder,
    private _userService: UserService,
    private alertService: AlertService

  ) {
    this.user = Helper.getUserFromLocalStorage() || new UserResponseModel();
    this.form = this.formBuilder.formGroup(UserDetailsModel, this.user);
  }
  ngOnInit() {
    this.setupData();
   }

   turnOffWarning() {
    console.log('turn off warning');
    
    this.showWarning = false;
   }

  setupData() {
    this.formGender()
    this.form?.get('dateOfBirth')?.setValue(TuiDay.fromLocalNativeDate(new Date(this.user.dateOfBirth)));
  }

  formGender() {
    const gender = this?.form?.get('gender')?.value;
    if (gender === 0) {
      this.form?.get('gender')?.setValue('Male')
    }
    if (gender === 1) {
      this.form?.get('gender')?.setValue('Female')
    }
  }

  editAvatar() {

  }

  formatGender(value: string): any {
    if (value === 'Male') {
      this?.form?.get('gender')?.setValue(0)
    }
    if (value === 'Female') {
      this?.form?.get('gender')?.setValue(1)
    }
  }

  get f() {
    return this.form.controls;
  }

  saveProfile() {
    if (this?.form?.valid) {
      this.formatGender(this.form.get('gender')?.value);
      this.isLoading = true;
      this._userService.editUserProfile(this.form.value).subscribe({
        next: (res) => {
          if (res) {
            this.user = res?.result;
            Helper.setUserToLocalStorage(this.user);
            this.form = this.formBuilder.formGroup(UserDetailsModel, this.user);
            this.alertService.showSuccess('Profile updated successfully', 'Success');
            this.isLoading = false;
          }
        },
        error: (err) => {
          console.log(err);
          this.alertService.showError('Update profile of user failed', 'Error');
          this.isLoading = false;
        },
        complete: () => {
          this.setupData();
          this.isLoading = false;
        }
      })

    }
  }
}
