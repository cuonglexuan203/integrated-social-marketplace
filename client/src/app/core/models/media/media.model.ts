export class MediaModel {
    publicId: string;
    url: string;
    contentType: string;
    fileSize: number;
    format: string;
    duration: number | null;
    thumbnailUrl: string | null;
    width: number;
    height: number;
}