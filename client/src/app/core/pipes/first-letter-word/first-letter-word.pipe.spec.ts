import { FirstLetterWordPipe } from './first-letter-word.pipe';

describe('FirstLetterWordPipe', () => {
  it('create an instance', () => {
    const pipe = new FirstLetterWordPipe();
    expect(pipe).toBeTruthy();
  });
});
