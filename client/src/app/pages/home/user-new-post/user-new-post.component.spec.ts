import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserNewPostComponent } from './user-new-post.component';

describe('UserNewPostComponent', () => {
  let component: UserNewPostComponent;
  let fixture: ComponentFixture<UserNewPostComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserNewPostComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserNewPostComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
