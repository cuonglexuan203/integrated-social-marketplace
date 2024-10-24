import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-blank',
  standalone: true,
  imports: [RouterOutlet,
    CommonModule
  ],
  templateUrl: './blank.component.html',
})
export class BlankComponent {

}
