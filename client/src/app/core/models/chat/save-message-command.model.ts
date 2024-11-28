interface SaveMessageCommand {
    roomId: string;
    senderId: string;
    contentText?: string;
    media?: File[]; // Assuming File type corresponds to IFormFile in C#
    attachedPostIds?: string[];
}
