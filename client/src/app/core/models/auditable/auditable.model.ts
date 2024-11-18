export class AuditableModel {
    createdAt: string;
    modifiedAt: string | null;
    isDeleted: boolean;
    deletedAt: string | null;
}