import { OrderByPipe } from './orderby.pipe';

describe('OrderbyPipe', () => {
  it('create an instance', () => {
    const pipe = new OrderByPipe();
    expect(pipe).toBeTruthy();
  });
});
