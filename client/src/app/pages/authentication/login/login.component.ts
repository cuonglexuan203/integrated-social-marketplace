import { Component } from '@angular/core';
import { LOGO_LOGIN_LEFT_SIDE, LOGO_LOGIN_RIGHT_SIDE } from './login-icon-data';
import { TuiIcon, TuiHint, TuiError } from '@taiga-ui/core';
import { TuiInputModule, TuiInputPasswordModule, TuiTextfieldControllerModule } from '@taiga-ui/legacy';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { LoginModel } from '../../../core/models/login/login.model';
import { RxFormBuilder, RxReactiveFormsModule } from '@rxweb/reactive-form-validators';
import { NbAuthService } from '@nebular/auth';
import { FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AlertService } from '../../../core/services/alert/alert.service';
import { TuiCheckbox } from '@taiga-ui/kit';
import { HINT_LOGIN } from '../../../core/constances/hint-login';
import { Helper } from '../../../core/utils/helper';


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [TuiIcon,
    TuiInputModule,
    CommonModule,
    TuiInputPasswordModule,
    FormsModule,
    ReactiveFormsModule,
    RxReactiveFormsModule,
    TuiCheckbox,
    TuiTextfieldControllerModule,
    TuiHint,
    TuiError
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  isLoading = false;
  logoLoginLeftSide = LOGO_LOGIN_LEFT_SIDE;
  logoLoginRightSide = LOGO_LOGIN_RIGHT_SIDE;
  form!: FormGroup;
  logoPath = 'assets/images/icons/appicon.svg';
  hintLogin = HINT_LOGIN;
  constructor(
    private router: Router,
    private authService: NbAuthService,
    private formBuilder: RxFormBuilder,
    private alertService: AlertService,

  ) {
    this.form = this.formBuilder.formGroup(LoginModel, {});
  }
  ngOnInit() {
  }

  get f() {
    return this.form.controls;
  }

  onClickNavigateRegister() {
    this.router.navigate(['/register']);
  }



  onSubmit() {
    if (this.form.valid) {
      Helper.clearLocalStorage();
      this.isLoading = true;
      this.authService.authenticate('email', this.form.value).subscribe(result => {
        console.log(result);
        if(result.isSuccess()) {
          this.isLoading = false;
          this.alertService.showSuccess('Success','Login Successfully');
          this.router.navigate(['/home']);
        }
        else {
          this.isLoading = false;
        }
      })
      
    }
  }

}
