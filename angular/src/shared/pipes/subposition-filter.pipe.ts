import { Pipe, PipeTransform } from '@angular/core';
import { SubPositionTalentDto } from '@app/service/model/talent.dto';

@Pipe({
  name: 'subpositionFilter'
})
export class SubpositionFilterPipe implements PipeTransform {
  transform(value: any[], property: string): any {
    console.log(value, property);
    if (!property) return value;
    return value.filter(s => s?.subPosition?.toLowerCase().includes(property.toLowerCase()));
  }

}
