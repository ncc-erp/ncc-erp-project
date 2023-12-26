import { APP_ENUMS } from "../AppEnums";
import { Pipe, PipeTransform } from "@angular/core";
@Pipe({
  name: "projectStatusPipe",
})

export class ProjectStatusPipe implements PipeTransform {
  transform(value: number) {
    switch (value) {
      case APP_ENUMS.ProjectStatus.Closed:
        return "<span class='badge badge-w-close badge-pill badge-secondary'>Closed</span>";
    }
  }
}
