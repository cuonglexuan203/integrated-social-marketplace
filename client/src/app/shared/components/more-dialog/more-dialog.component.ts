import { Component } from '@angular/core';
import { MoreDialogData } from '../../../core/data/more-dialog-data';

@Component({
  selector: 'app-more-dialog',
  standalone: true,
  imports: [],
  templateUrl: './more-dialog.component.html',
  styleUrl: './more-dialog.component.css'
})
export class MoreDialogComponent {
  moreDialogData = MoreDialogData;
  constructor() {}
  ngOnInit() {}
}
