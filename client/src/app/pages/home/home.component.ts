import { Component, Output } from '@angular/core';
import { PostItemComponent } from "./post-item/post-item.component";
import { MockDataPost } from '../../shared/mocks/mock-data-post';
import { FilterComponent } from '../../shared/components/filter/filter.component';
import { FilterData } from '../../core/data/filter-data';
import { Filter } from '../../core/models/filter/filter.model';
import { TagItemComponent } from './tag-item/tag-item.component';
@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    PostItemComponent,
    TagItemComponent,
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  @Output() mockData: MockDataPost[] = [];
  constructor() { }

  ngOnInit() {

  }

  mockDataPost(): any {
    let mockDataPostArray: MockDataPost[] = [];
    const mockDataPost: MockDataPost = {
      id: 1,
      userId: 123,
      name: "Sample Post",
      type: "image", // example post type
      description: "This is a sample post description for testing.",
      mediaUrl: [
        {
          imageUrl: ["http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/images/BigBuckBunny.jpg", "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/images/ElephantsDream.jpg"],
          videoUrl: ["http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4"]
        }
      ],
      likesCount: [
        {
          id: 1,
          postId: 1,
          userId: 234,
          createdAt: "2024-11-07T10:00:00Z",
          updatedAt: "2024-11-07T12:00:00Z",
          isDeleted: false,
          deletedAt: ""
        },
        {
          id: 2,
          postId: 1,
          userId: 235,
          createdAt: "2024-11-07T11:00:00Z",
          updatedAt: "2024-11-07T13:00:00Z",
          isDeleted: false,
          deletedAt: ""
        }
      ],
      commentsCount: [
        {
          id: 1,
          postId: 1,
          userId: 234,
          commentText: "Great post!",
          createdAt: "2024-11-07T10:05:00Z",
          updatedAt: "2024-11-07T10:10:00Z",
          isDeleted: false,
          deletedAt: "",
          parentCommentId: 0
        },
        {
          id: 2,
          postId: 1,
          userId: 236,
          commentText: "Thanks for sharing.",
          createdAt: "2024-11-07T11:20:00Z",
          updatedAt: "2024-11-07T11:25:00Z",
          isDeleted: false,
          deletedAt: "",
          parentCommentId: 0
        }
      ],
      sharesCount: [
        {
          id: 1,
          postId: 1,
          userId: 237,
          createdAt: "2024-11-07T11:30:00Z",
          updatedAt: "2024-11-07T11:35:00Z",
          isDeleted: false,
          deletedAt: ""
        }
      ],
      createdAt: "2024-11-07T10:00:00Z",
      updatedAt: "2024-11-07T12:00:00Z",
      isDeleted: false,
      deletedAt: "",
      price: 19.99
    };

    mockDataPostArray.push(mockDataPost);
    mockDataPostArray.push(mockDataPost);


    return mockDataPostArray;
  }


}
