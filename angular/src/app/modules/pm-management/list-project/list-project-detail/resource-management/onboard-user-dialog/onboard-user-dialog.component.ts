import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-onboard-user-dialog',
  templateUrl: './onboard-user-dialog.component.html',
  styleUrls: ['./onboard-user-dialog.component.css']
})
export class OnboardUserDialogComponent implements OnInit {
  isAdmin: boolean = false;
  anyChangeToSave: boolean = false;
  checklist = [
    {
      title: 'PM tạo meeting onboarding',
      steps: [
        'Việc giới thiệu new member, all old member thường ở daily meeting',
        'PM giới thiệu hoặc member tự giới thiệu các thông tin: tên, tuổi, văn phòng, vị trí (dev fe, dev be, dev fullstack, qa, lead fe, ..)'
      ],
      done: false
    },
    {
      title: 'PM nói expect đối với new member',
      steps: [
        'Mong muốn nhân viên có thái độ, cách làm việc, mức độ perform như thế nào,...'
      ],
      done: false
    },
    {
      title: 'PM yêu cầu member đọc kỹ <a href="https://docs.google.com/spreadsheets/d/114Bsif-rDnwDdTkVrqPZFAE3tEe4Y7ULPg5PHU1qATY/edit?gid=0#gid=0">Các lỗi ngớ ngẩn gây hậu quả nghiêm trọng mà member tuyệt đối cần tránh khi làm dự án</a>.',
      steps: [
        'Cái này rất quan trọng, PM cần yêu cầu member dành ra tối thiểu 30 phút để đọc kỹ.',
        'Điều nào không hiểu thì phải hỏi lại ngay.',
        'PM cần hỏi lại member xem đã hiểu hết chưa, còn chỗ nào nào thắc mắc không.',
        'Chỗ nào member chưa hiểu thì PM cần giải thích cho đến khi member hiểu rõ.'
      ],
      done: false
    },
    {
      title: 'PM nói rõ cho new member về account, stakeholder',
      steps: [
        'PM share thông tin account trên passbolt cho member',
        'Nói rõ cho member: mình đại diện cho bên nào',
        'PM share thông tin internal team, external team (trên ops) cho member'
      ],
      done: false
    },
    {
      title: 'PM phổ biến cho member quy trình làm task',
      steps: [
        'nhận task ở đâu, từ ai',
        'kéo task (todo, inprogress, PR review, ....)',
        'clear/confirm req của task trên đâu (notion/jira, ...)'
      ],
      done: false
    },
    {
      title: 'PM phổ biến cho member quy trình git flow của dự án',
      steps: [
        'PM cần đưa quy trình git flow lên OPS và gửi link cho member đọc'
      ],
      done: false
    },
    {
      title: 'PM assign mentor cho new member',
      steps: [
        'Intern bắt buộc phải có mentor'
      ],
      done: false
    }
  ];

  status: string = 'Draft';
  onboardCode: string = '';
  hasAnyChange: boolean = false;


  constructor() { }

  ngOnInit(): void {
  }

  saveDraft() {
    // Add your logic here
  }

  confirmOnboard() {
    // Add your logic here
  }
}
