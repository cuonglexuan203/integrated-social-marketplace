import { Component, Output } from '@angular/core';
import { PostItemComponent } from "./post-item/post-item.component";
import { MockDataPost } from '../../shared/mocks/mock-data-post';
import { FilterComponent } from '../../shared/components/filter/filter.component';
import { FilterData } from './filter-data';
import { Filter } from '../../core/models/filter/filter.model';
@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    PostItemComponent,
    FilterComponent
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  @Output() mockData: MockDataPost[] = [];
  filters: Filter[] = FilterData;
  constructor() { }

  ngOnInit() {
    this.mockData = this.generateMockData(50);

  }
  generateMockData(count: number): MockDataPost[] {
    const mockData: MockDataPost[] = [];

    for (let i = 1; i <= count; i++) {
      const now = new Date();
      const hoursAgo = Math.floor(Math.random() * 100); // Randomly subtract up to 100 hours
      const updatedAt = new Date(now.getTime() - hoursAgo * 60 * 60 * 1000); // Subtract hours
      const mockItem: MockDataPost = {
        id: i,
        description: `Description for post ${i}`,
        name: `Car ${i}`,
        type: `mercedes ${i}`,
        mediaUrl: `https://example.com/media/${i}`,
        likesCount: Math.floor(Math.random() * 1000),
        commentsCount: Math.floor(Math.random() * 100),
        createdAt: now.toISOString(),
        updatedAt: updatedAt.toISOString(),
        isDeleted: false,
        deletedAt: '',
        price: parseFloat((Math.random() * 100 + 1).toFixed(2)),
      };
      mockData.push(mockItem);
    }

    return mockData;
  }

}
