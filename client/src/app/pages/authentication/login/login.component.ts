import { Component } from '@angular/core';
import { LOGO_LOGIN_LEFT_SIDE, LOGO_LOGIN_RIGHT_SIDE } from './login-icon-data';
import { TuiIcon } from '@taiga-ui/core';
import { TuiInputModule, TuiInputPasswordModule } from '@taiga-ui/legacy';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-login',
  standalone: true,
  imports: [TuiIcon,
    TuiInputModule,
    CommonModule,
    TuiInputPasswordModule,
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  logoLoginLeftSide = LOGO_LOGIN_LEFT_SIDE;
  logoLoginRightSide = LOGO_LOGIN_RIGHT_SIDE;

  constructor(
    private router: Router,

  ) { }
  ngOnInit() { }

  onClickRegister() {
    this.router.navigate(['/register']);
  }

}
