import { Component } from '@angular/core';
import { LOGO_LOGIN_LEFT_SIDE } from '../login/login-icon-data';
import { TuiError, TuiHint, TuiIcon } from '@taiga-ui/core';
import { TuiInputModule, TuiInputPasswordModule, TuiTextfieldControllerModule } from '@taiga-ui/legacy';
import { CommonModule } from '@angular/common';
import { FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RxFormBuilder, RxReactiveFormsModule } from '@rxweb/reactive-form-validators';
import { TuiCheckbox } from '@taiga-ui/kit';
import { AlertService } from '../../../core/services/alert/alert.service';
import { NbAuthService } from '@nebular/auth';
import { RegisterModel } from '../../../core/models/register/register.model';
import { Router } from '@angular/router';
import { HINT_REGISTER } from '../../../core/constances/hint-register';
import { LottieComponent, AnimationOptions } from 'ngx-lottie';
import { UserService } from '../../../core/services/user/user.service';
import { AuthService } from '../../../core/services/auth/auth.service';
import type { TuiCountryIsoCode } from '@taiga-ui/i18n';
import {
  TuiInputPhoneInternational,
  tuiInputPhoneInternationalOptionsProvider,
  TuiSortCountriesPipe,
} from '@taiga-ui/kit';
import { getCountries } from 'libphonenumber-js';
import { defer } from 'rxjs';
import {AsyncPipe} from '@angular/common';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    TuiInputModule,
    CommonModule,
    TuiInputPasswordModule,
    FormsModule,
    ReactiveFormsModule,
    RxReactiveFormsModule,
    TuiCheckbox,
    TuiIcon,
    TuiTextfieldControllerModule,
    TuiHint,
    TuiError,
    LottieComponent,
    TuiInputPhoneInternational,
    TuiSortCountriesPipe,
    AsyncPipe
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
  providers: [
    tuiInputPhoneInternationalOptionsProvider({
        metadata: defer(async () =>
            import('libphonenumber-js/max/metadata').then((m) => m.default),
        ),
    }),
],
})
export class RegisterComponent {
  isLoading: boolean = false;

  logoPath = '../../../../assets/images/icons/appicon.svg';
  logoLoginLeftSide = LOGO_LOGIN_LEFT_SIDE;
  form!: FormGroup;
  hintRegister = HINT_REGISTER;

  options: AnimationOptions = {
    path: 'assets/animations/login.json',
    loop: true,
  };

  constructor(
    private router: Router,
    private formBuilder: RxFormBuilder,
    private alertService: AlertService,
    private _userService: UserService,
    private authService: AuthService,
  ) {
    this.form = this.formBuilder.formGroup(RegisterModel, {});

  }

  protected readonly countries = getCountries();
  protected countryIsoCode: TuiCountryIsoCode = 'VN';
  protected value = '';


  ngOnInit() { }

  get f() {
    return this.form.controls;
  }

  onClickNavigateLogin() {
    this.router.navigate(['/login']);
  }

  onSubmit() {
    const roles = ['user'];
    this.form.controls['roles']?.setValue(roles);

    if (this?.form?.valid) {
      this.isLoading = true;
      this.authService.register(this.form.value).subscribe({
        next: (res) => {
          if (res) {
            this.alertService.showSuccess('User registered successfully', 'Success');
            this.router.navigate(['/login']);
          }
        },
        error: (err) => {
          this.alertService.showError('Error while registering user', 'Error');
          this.isLoading = false;
        },
        complete: () => {
          this.isLoading = false;
        }
      });
    }
    else {
      this.form.markAllAsTouched();
    }
  }

}
