import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TuiRoot } from '@taiga-ui/core';

@Component({
  selector: 'app-blank',
  standalone: true,
  imports: [RouterOutlet,
    CommonModule,
    TuiRoot
  ],
  templateUrl: './blank.component.html',
})
export class BlankComponent {

}
