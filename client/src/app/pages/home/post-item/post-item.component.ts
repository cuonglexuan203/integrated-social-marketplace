import { Component, Input } from '@angular/core';
import { TuiButton, TuiIcon, TuiRoot } from '@taiga-ui/core';
import { TuiAvatar, TuiCarousel } from '@taiga-ui/kit';
import { TuiCardLarge } from '@taiga-ui/layout';
import { MockDataPost } from '../../../shared/mocks/mock-data-post';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-post-item',
  standalone: true,
  imports: [
    TuiAvatar,
    CommonModule,
    TuiIcon,
    TuiCarousel,
    TuiButton,
  ],
  templateUrl: './post-item.component.html',
  styleUrl: './post-item.component.css'
})
export class PostItemComponent {
  @Input() item: MockDataPost;
  currentIndex: number = 0;


  constructor() { }

  ngOnInit() {
    
  }

  onCarouselChange(index: number) {
    this.currentIndex = index;
  }

  
}
  
