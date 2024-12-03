export class ReportPostModel {
    postId: string;
    reporterId: string;
    targetUserId: string;
    contentText: string;
    reportType: 0;
    reporterCredibilityScore: 0;
}

export class UpdateValidityModel {
    reportId: string;
    validity: boolean;
}