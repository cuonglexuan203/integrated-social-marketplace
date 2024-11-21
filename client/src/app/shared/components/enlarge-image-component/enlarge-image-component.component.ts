import { Component, ViewEncapsulation } from '@angular/core';
import { TuiDialogContext } from '@taiga-ui/core';
import { injectContext } from '@taiga-ui/polymorpheus';
import { MediaModel } from '../../../core/models/media/media.model';

@Component({
  selector: 'app-enlarge-image-component',
  standalone: true,
  imports: [],
  templateUrl: './enlarge-image-component.component.html',
  styleUrl: './enlarge-image-component.component.css',
  encapsulation: ViewEncapsulation.None,
})
export class EnlargeImageComponentComponent {
  public readonly context = injectContext<TuiDialogContext<any>>();
  data: any;
  media: MediaModel
  constructor() {
    this.data = this.context.data;
    
   }

  ngOnInit() {
    this.media = this.data;
    console.log(this.media);
    
  }
}
