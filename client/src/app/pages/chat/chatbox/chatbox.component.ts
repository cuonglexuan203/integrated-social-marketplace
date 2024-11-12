import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TuiIcon, TuiTextfield } from '@taiga-ui/core';
import { TuiAvatar, TuiBadgedContent, TuiSwitch } from '@taiga-ui/kit';
import { TuiCell, TuiSearch } from '@taiga-ui/layout';
import { ChatBoxHeaderData } from '../../../core/data/chatbox-header-data';
import { TuiInputModule } from '@taiga-ui/legacy';

@Component({
  selector: 'app-chatbox',
  standalone: true,
  imports: [
    TuiIcon,
    TuiTextfield,
    TuiSearch,
    TuiAvatar,
    TuiBadgedContent,
    TuiCell,
    TuiIcon,
    CommonModule,
    TuiSwitch,
    FormsModule,
    ReactiveFormsModule,
    TuiInputModule
  ],
  templateUrl: './chatbox.component.html',
  styleUrl: './chatbox.component.css'
})
export class ChatboxComponent {
  chatBoxHeaderData = ChatBoxHeaderData;
  valueMessage = '';

  constructor() {}
  ngOnInit() {}
}
