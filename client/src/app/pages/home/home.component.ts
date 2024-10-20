import { Component } from '@angular/core';
import { PostItemComponent } from "./post-item/post-item.component";

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [PostItemComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

}
