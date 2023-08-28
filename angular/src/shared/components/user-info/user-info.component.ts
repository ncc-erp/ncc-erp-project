import { UploadAvatarComponent } from './../../../app/users/upload-avatar/upload-avatar.component';
import { AppComponentBase } from '@shared/app-component-base';
import { UserService } from './../../../app/service/api/user.service';
import { UserDto } from './../../service-proxies/service-proxies';
import { Component, OnInit, Input, Injector } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';

@Component({
  selector: 'app-user-info',
  templateUrl: './user-info.component.html',
  styleUrls: ['./user-info.component.css']
})
export class UserInfoComponent extends AppComponentBase implements OnInit {
  @Input() userData: UserDto
  @Input() workType:string;
  @Input() isPool:boolean
  @Input() averagePoint: number
  @Input() isWeeklyReport: boolean;
  public user: UserDto
  constructor(injector: Injector) {
    super(injector)
  }
  ngOnInit(): void {
  }

  ngOnChanges(): void {
    this.user = this.userData
  }
  public getProjectTypefromEnum(projectType: number, enumObject: any) {
    for (const key in enumObject) {
      if (enumObject[key] == projectType) {
        return key;
      }
    }
  }

  public getStarColor(star, isClass){
    switch(star){
      case 1:  case 2: return isClass ? 'col-grey' : '#9E9E9E'
      case 3: return isClass ? 'col-gold' : 'gold'
      case 4: return isClass ? 'col-orange' : '#FF9800'
      case 5: return isClass ? 'col-red' : '#F44336'
      default: return ''
    }

  }

  getUserLevel() {
    switch (this.user.userLevel) {
      case 0:
        return {
          userLevel: "Intern_0",
          style: {'background-color': '#B2BEB5'}
        }
      case 1:
        return {
          userLevel: "Intern_1",
          style: {'background-color': '#8F9779'}
        }
      case 2:
        return {
          userLevel: "Intern_2",
          style: {'background-color': '#665D1E', 'color': 'white'}
        }
      case 3: {
        return {
          userLevel: "Intern_3",
          style: {'background-color': '#777'}
        }
      }
      case 4: {
        return {
          userLevel: "Fresher-",
          style: {'background-color': '#60b8ff'}
        }
      }
      case 5: {
        return {
          userLevel: "Fresher",
          style: {'background-color': '#318CE7'}
        }
      }
      case 6: {
        return {
          userLevel: "Fresher+",
          style: {'background-color': '#1f75cb'}
        }
      }
      case 7: {
        return {
          userLevel: "Junior-",
          style: {'background-color': '#ad9fa1'}
        }
      }
      case 8: {
        return {
          userLevel: "Junior",
          style: {'background-color': '#A57164'}
        }
      }
      case 9: {
        return {
          userLevel: "Junior+",
          style: {'background-color': '#3B2F2F'}
        }
      }
      case 10: {
        return {
          userLevel: "Middle-",
          style: {'background-color': '#A4C639'}
        }
      }
      case 11: {
        return {
          userLevel: "Middle",
          style: {'background-color': '#3bab17'}
        }
      }
      case 12: {
        return {
          userLevel: "Middle+",
          style: {'background-color': '#008000'}
        }
      }
      case 13: {
        return {
          userLevel: "Senior-",
          style: {'background-color': '#c36285'}
        }
      }
      case 14: {
        return {
          userLevel: "Senior",
          style: {'background-color': '#AB274F'}
        }
      }
      case 15: {
        return {
          userLevel: "Principal ",
          style: {'background-color': '#902ee1'}
        }
      }
    }
  }
  public getStarColorforReviewInternCapability(average, isClass) {
    if (average < 2.5) {
      return 'grey'
    }
    if (average < 3.5) {
      return 'yellow'
    }
    if (average < 4.5) {
      return 'orange'
    }
    else {
      return ''
    }
  }
}

