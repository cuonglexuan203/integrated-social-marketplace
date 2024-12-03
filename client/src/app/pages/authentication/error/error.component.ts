import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { TuiIcon } from '@taiga-ui/core';
import { AnimationOptions, LottieComponent } from 'ngx-lottie';

@Component({
  selector: 'app-error',
  standalone: true,
  imports: [
    LottieComponent,
    TuiIcon
  ],
  templateUrl: './error.component.html',
  styleUrl: './error.component.css'
})
export class ErrorComponent {

  options: AnimationOptions = {
    path: 'assets/animations/access-denied.json',
    loop: true,
  };
  constructor(
    private router: Router
  ) { }

  ngOnInit() {
  }

  back() {
    this.router.navigate(['/home']);
  }
}
