export interface User {
    id: string;
    fullName: string;
    userName: string;
    email: string;
    roles: string[];
    profilePictureUrl: string;
    profileUrl: string;
}

export interface Media {
    publicId: string;
    url: string;
    contentType: string;
    fileSize: number;
    format: string;
    duration: number | null;
    thumbnailUrl: string;
    width: number;
    height: number;
}

export interface Reaction {
    user: User;
    type: string;
    createdAt: string;
}

export interface Comment {
    id: string;
    user: User;
    contentText: string;
    media?: Media[];
    likesCount: number;
    reactions?: Reaction[];
    parentCommentId?: string;
    createdAt: string;
    modifiedAt: string;
}

export interface FeedPost {
    id: string;
    user: User;
    contentText: string;
    media: Media[];
    likesCount: number;
    reactions?: Reaction[];
    commentsCount: number;
    comments?: Comment[];
    link: string;
    sharedPostId: string | null;
    tags: string[];
    createdAt: string;
    modifiedAt: string;
}