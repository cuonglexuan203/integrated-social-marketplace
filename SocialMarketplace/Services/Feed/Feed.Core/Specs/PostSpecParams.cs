﻿namespace Feed.Core.Specs
{
    public class PostSpecParams
    {
        private const int _maxPageSize = 100;
        private int _pageSize = 5;
        public int PageSize { get => _pageSize; set => _pageSize = value > _maxPageSize ? _maxPageSize : value; }
        public int PageIndex { get; set; } = 1;
        public string? Sort { get; set; }
    }
}