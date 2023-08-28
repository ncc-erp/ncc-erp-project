import { ProjectManagementTemplatePage } from './app.po';

describe('ProjectManagement App', function() {
  let page: ProjectManagementTemplatePage;

  beforeEach(() => {
    page = new ProjectManagementTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
