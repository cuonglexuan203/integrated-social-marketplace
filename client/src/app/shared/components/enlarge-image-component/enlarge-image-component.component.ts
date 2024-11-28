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
  type: string;
  isLoading: boolean = false;
  constructor() {
    this.data = this.context.data;
   }

  ngOnInit() {
    this.media = this.data.media;
    this.type = this.data.type;
    this.handleMedia(this.media);
  }

  handleMedia(media: MediaModel) {
    this.isLoading = true;
    if (this.type === 'cloudinary') {
      this.media = {
        ...media,
        url: this.handleUrlLargeMedia(media.url)
      }
    }
    this.isLoading = false;
  }

  handleUrlLargeMedia(url: string): string {
    return url.replace('/upload/', '/upload/w_1000/');
  }
}
