import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TuiIcon } from '@taiga-ui/core';
import { TuiAvatar, TuiBadge, TuiBadgedContent, TuiSwitch } from '@taiga-ui/kit';
import { TuiCardLarge, TuiCell } from '@taiga-ui/layout';

@Component({
  selector: 'app-tag-item',
  standalone: true,
  imports: [
    FormsModule,
    TuiAvatar,
    TuiBadge,
    TuiBadgedContent,
    TuiCell,
    TuiIcon,
    TuiSwitch,
  ],
  templateUrl: './tag-item.component.html',
  styleUrl: './tag-item.component.css'
})
export class TagItemComponent {
  protected incoming = false;
  protected outgoing = true;
}
