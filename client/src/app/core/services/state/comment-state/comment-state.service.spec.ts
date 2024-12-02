import { TestBed } from '@angular/core/testing';

import { CommentStateService } from './comment-state.service';

describe('CommentStateService', () => {
  let service: CommentStateService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CommentStateService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
