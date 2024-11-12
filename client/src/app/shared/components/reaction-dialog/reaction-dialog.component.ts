import { Component } from '@angular/core';
import { TuiHint } from '@taiga-ui/core';
import {TuiTooltip} from '@taiga-ui/kit';

@Component({
  selector: 'app-reaction-dialog',
  standalone: true,
  imports: [
    TuiHint,
    TuiTooltip,
  ],
  templateUrl: './reaction-dialog.component.html',
  styleUrl: './reaction-dialog.component.css'
})
export class ReactionDialogComponent {
  reactions = [
    { emoji: '👍', label: 'Like' },
    { emoji: '❤️', label: 'Love' },
    { emoji: '😮', label: 'Wow' },
    { emoji: '😢', label: 'Sad' },
    { emoji: '😠', label: 'Angry' }
  ];

  constructor() {
  }
  
  ngOnInit() {
  }


}
