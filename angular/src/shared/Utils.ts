import * as moment from 'moment';
export class Utils {
  public static getUserLevel(level) {
    switch (level) {
      case 0:
        return {
          userLevel: "Intern_0",
          style: { 'background-color': '#B2BEB5' }
        }
      case 1:
        return {
          userLevel: "Intern_1",
          style: { 'background-color': '#8F9779' }
        }
      case 2:
        return {
          userLevel: "Intern_2",
          style: { 'background-color': '#665D1E', 'color': 'white' }
        }
      case 3: {
        return {
          userLevel: "Intern_3",
          style: { 'background-color': '#777' }
        }
      }
      case 4: {
        return {
          userLevel: "Fresher-",
          style: { 'background-color': '#60b8ff' }
        }
      }
      case 5: {
        return {
          userLevel: "Fresher",
          style: { 'background-color': '#318CE7' }
        }
      }
      case 6: {
        return {
          userLevel: "Fresher+",
          style: { 'background-color': '#1f75cb' }
        }
      }
      case 7: {
        return {
          userLevel: "Junior-",
          style: { 'background-color': '#ad9fa1' }
        }
      }
      case 8: {
        return {
          userLevel: "Junior",
          style: { 'background-color': '#A57164' }
        }
      }
      case 9: {
        return {
          userLevel: "Junior+",
          style: { 'background-color': '#3B2F2F' }
        }
      }
      case 10: {
        return {
          userLevel: "Middle-",
          style: { 'background-color': '#A4C639' }
        }
      }
      case 11: {
        return {
          userLevel: "Middle",
          style: { 'background-color': '#3bab17' }
        }
      }
      case 12: {
        return {
          userLevel: "Middle+",
          style: { 'background-color': '#008000' }
        }
      }
      case 13: {
        return {
          userLevel: "Senior-",
          style: { 'background-color': '#c36285' }
        }
      }
      case 14: {
        return {
          userLevel: "Senior",
          style: { 'background-color': '#AB274F' }
        }
      }
      case 15: {
        return {
          userLevel: "Principal ",
          style: { 'background-color': '#902ee1' }
        }
      }
      default:
        return {
          userLevel: "Any Level",
          style: { 'background-color': '#17a2b8' }
        }
    }
  }

  public static getTimeHistory(timeHistory: any) {
    const now = Date.now()
    timeHistory = moment(timeHistory).format('YYYY-MM-DD')
    let time;
    if (now < new Date(timeHistory).getTime()) {
      time = Math.round((new Date(timeHistory).getTime() - now) / (1000 * 3600 * 24));
      if (time < 1) {
        return ''
      }
      if (time == 1) {
        return 'in 1 day';
      }
      else if (time <= 7) {
        return 'in ' + time + ' days';
      }
      else if (time <= 30) {
        let week = Math.round(time / 7)
        return week > 1 ? 'in ' + week + ' weeks' : 'in ' + week + ' week'
      }
      else if (time <= 365) {
        let month = Math.round(time / 30)
        return month > 1 ? 'in ' + month + ' months' : 'in ' + month + ' month'
      }
      else {
        let year = Math.round(time / 365)
        return year > 1 ? 'in ' + year + ' years' : 'in ' + year + ' year'
      }
    }
    else {
      time = Math.round((now - new Date(timeHistory).getTime()) / (1000 * 3600 * 24));
      if (time < 1) {
        return ''
      }
      if (time == 1) {
        return '1 day ago';
      }
      else if (time <= 7) {
        return time + ' days ago';
      }
      else if (time <= 30) {
        let week = Math.round(time / 7)
        return week > 1 ? week + ' weeks ago' : week + ' week ago'
      }
      else if (time <= 365) {
        let month = Math.round(time / 30)
        return month > 1 ? month + ' months ago' : month + ' month ago'
      }
      else {
        let year = Math.round(time / 365)
        return year > 1 ? year + ' years ago' : year + ' year ago'
      }
    }
  }

  public static getProjectTypefromEnum(projectType: number, enumObject: any) {
    for (const key in enumObject) {
      if (enumObject[key] == projectType) {
        return key;
      }
    }
  }

  public static getByEnum(enumValue: number, enumObject: any) {
    for (const key in enumObject) {
      if (enumObject[key] == enumValue) {
        return key;
      }
    }
  }

  public static changeTextProjectType(projectType: string) {
    return projectType === 'TAM' ? 'T&M' : projectType
  }

  public static getValueByEnum(enumValue: number, enumObject) {
    for (const key in enumObject) {
      if (enumObject[key] == enumValue) {
        return key;
      }
    }
  }
}
