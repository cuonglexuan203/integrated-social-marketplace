import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NavItem } from './nav-item';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { TuiIcon, TuiHint } from '@taiga-ui/core';
import { TuiTooltip } from '@taiga-ui/kit';

@Component({
  selector: 'app-nav-item',
  standalone: true,
  imports: [CommonModule,
    RouterModule,
    TuiIcon,
    TuiTooltip,
    TuiHint
  ],
  templateUrl: './nav-item.component.html',
  styleUrl: './nav-item.component.css'
})
export class NavItemComponent {
  @Input() item: NavItem;
  @Input() isToggle: boolean;
  constructor() {}

  ngOnInit() {
  }
  
}
