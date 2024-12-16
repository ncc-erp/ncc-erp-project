import { Injectable } from '@angular/core';
import * as FileSaver from 'file-saver';

@Injectable({
  providedIn: 'root'
})
export class FileHandlerService {

  constructor() { }

  downloadFile(base64Data: string, fileName: string, mimeType: string = 'application/octet-stream'): void {
    const byteArray = this.s2ab(atob(base64Data));
    const fileBlob = new Blob([byteArray], { type: mimeType });
    FileSaver.saveAs(fileBlob, fileName);
  }

  private s2ab(s: string): ArrayBuffer {
    const byteArray = new Uint8Array(s.length);
    for (let i = 0; i < s.length; i++) {
      byteArray[i] = s.charCodeAt(i) & 0xFF;
    }
    return byteArray.buffer;
  }
}
