import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TuiButton, TuiDialogContext } from '@taiga-ui/core';
import { injectContext } from '@taiga-ui/polymorpheus';
import { TuiRadioList } from '@taiga-ui/kit';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-confirmation-dialog',
  standalone: true,
  imports: [CommonModule,
    TuiButton,
    TuiRadioList,
    FormsModule,
    ReactiveFormsModule,
  ],
  templateUrl: './confirmation-dialog.component.html',
  styleUrl: './confirmation-dialog.component.css'
})
export class ConfirmationDialogComponent {
  data: any;
  title: string = '';
  message: string = '';
  listSelection: string[] = [];
  selectedItem: string | null = null; // Variable to track selected radio item
  reportValidity: any;
  public readonly context = injectContext<TuiDialogContext<boolean>>();

  constructor() {
    this.data = this.context.data;
    this.title = this.data.title;
    this.message = this.data.message;
    this.listSelection = this.data.listSelection;
    this.reportValidity = this.data.reportValidity;
  }

  ngOnInit() {
    this.handleSelectedValidity();
  }

  handleSelectedValidity() {
    if (this.reportValidity !== null) {
      if (this.reportValidity === true) {
        this.selectedItem = this.listSelection[0];
      }
      else {
        this.selectedItem = this.listSelection[1];
      }
    }
    else {
      this.selectedItem = this.listSelection[0];
    }
  }

  onConfirm(): void {
    if (this.selectedItem) {
      if (this.selectedItem === 'Valid') {
        this.context.completeWith(true);
      }
      else {
        this.context.completeWith(false);
      }
    }
  }

  onSelectionChange() {
    if (this.listSelection) {
      this
    }
  }
}