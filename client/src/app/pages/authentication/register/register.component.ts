import { Component } from '@angular/core';
import { LOGO_LOGIN_LEFT_SIDE } from '../login/login-icon-data';
import { TuiHint, TuiIcon } from '@taiga-ui/core';
import { TuiInputModule, TuiInputPasswordModule, TuiTextfieldControllerModule } from '@taiga-ui/legacy';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RxReactiveFormsModule } from '@rxweb/reactive-form-validators';
import { TuiCheckbox } from '@taiga-ui/kit';

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
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  logoPath = '../../../../assets/images/icons/appicon.svg';
  logoLoginLeftSide = LOGO_LOGIN_LEFT_SIDE;
  constructor() { }
  ngOnInit() { }
}
