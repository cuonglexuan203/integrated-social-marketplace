import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrandingComponent } from './branding.component';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [
    CommonModule,
    BrandingComponent
  ],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent {

  constructor() { }


  ngOnInit() {}
}
