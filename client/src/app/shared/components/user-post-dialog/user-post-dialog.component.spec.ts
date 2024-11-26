import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserPostDialogComponent } from './user-post-dialog.component';

describe('UserPostDialogComponent', () => {
  let component: UserPostDialogComponent;
  let fixture: ComponentFixture<UserPostDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserPostDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserPostDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
