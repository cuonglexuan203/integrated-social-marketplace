export class Page {
    pageSize: number;
    pageIndex: number;
    sort: string | null;
}

export class PageUserDetail extends Page {
    userId: string;
}