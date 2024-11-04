import { Component, Input } from '@angular/core';
import { TuiIcon, TuiRoot } from '@taiga-ui/core';
import { TuiAvatar } from '@taiga-ui/kit';
import { TuiCardLarge } from '@taiga-ui/layout';
import { MockDataPost } from '../../../shared/mocks/mock-data-post';

@Component({
  selector: 'app-post-item',
  standalone: true,
  imports: [
    TuiAvatar,
    TuiCardLarge,
    TuiIcon,
  ],
  templateUrl: './post-item.component.html',
  styleUrl: './post-item.component.css'
})
export class PostItemComponent {
  @Input() item: MockDataPost;
  constructor() { }

  ngOnInit() {
  }

  
}
  
