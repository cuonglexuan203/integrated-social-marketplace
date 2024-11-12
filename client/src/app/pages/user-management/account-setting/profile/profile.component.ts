import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TuiButton, TuiDataList } from '@taiga-ui/core';
import {
  TuiAvatar, TuiDataListWrapper,
} from '@taiga-ui/kit';
import { TuiInputDateModule, TuiInputModule, TuiSelectModule, TuiTextareaModule } from '@taiga-ui/legacy';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    TuiAvatar,
    CommonModule,
    TuiButton,
    TuiInputModule,
    TuiTextareaModule,
    FormsModule,
    TuiSelectModule,
    TuiDataList,
    TuiDataListWrapper,
    TuiInputDateModule,
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent {
  genders: string[] = ['Male', 'Female', 'Different'];
  constructor() { }
  ngOnInit() { }
}
