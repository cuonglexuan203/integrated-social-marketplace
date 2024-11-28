import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RxFormBuilder, RxReactiveFormsModule } from '@rxweb/reactive-form-validators';
import { UserSecurityModel } from '../../../../core/models/user/user-security.model';
import { TuiInputModule, TuiInputPasswordModule, TuiTextfieldControllerModule } from '@taiga-ui/legacy';
import { TuiButton, TuiError, TuiIcon, TuiLoader } from '@taiga-ui/core';
import { AuthService } from '../../../../core/services/auth/auth.service';
import { Helper } from '../../../../core/utils/helper';
import { AlertService } from '../../../../core/services/alert/alert.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-security',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RxReactiveFormsModule,
    TuiInputPasswordModule,
    TuiTextfieldControllerModule,
    TuiInputModule,
    TuiIcon,
    TuiError,
    TuiButton,
    TuiLoader
  ],
  templateUrl: './security.component.html',
  styleUrl: './security.component.css'
})
export class SecurityComponent {
  isLoading: boolean = false;
  form!: FormGroup;
  constructor(
    private formBuilder: RxFormBuilder,
    private authService: AuthService,
    private alertService: AlertService,
    private router: Router
  ) { 
    this.form = this.formBuilder.formGroup(UserSecurityModel, {});

  }

  ngOnInit() {
  }

  get f() {
    return this.form.controls;
  }

  onSubmit() {
    if (this?.form?.valid) {
      this.isLoading = true;
      this.authService.changePassword(this.form.value).subscribe({
        next: (res) => {
          if (res?.result) {
            this.alertService.showSuccess('Password changed successfully', 'Success');
            this.isLoading = false;
            this.router.navigate(['/home']);
          }
        },
        error: (err) => {
          console.log(err);
          this.isLoading = false;
        },
        complete: () => {
          this.isLoading = false;
        }
      });
    }
  }
}
