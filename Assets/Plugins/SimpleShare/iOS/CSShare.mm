extern UIViewController* UnityGetGLViewController();

extern "C" void _NativeActivity( const char *filePath, char *title, const char *body, float x, float y )
{
    NSString *path = [NSString stringWithUTF8String:filePath];
    UIImage *image = [UIImage imageWithContentsOfFile:path];
    NSMutableArray *items  = [NSMutableArray arrayWithObject:
                              [NSString stringWithUTF8String:body]];
    NSLog(@"path: %@", path);
    if (image != nil)
    {
        [items addObject:image];
    }
    else
    {
        NSLog(@"image == nil");
    }
    
    UIActivityViewController *activity = [[UIActivityViewController alloc]
                                          initWithActivityItems:items
                                          applicationActivities:nil];
    
    UIViewController *rootViewController = UnityGetGLViewController();
    
    if( UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPhone )
    {
        [rootViewController presentViewController:activity animated:YES completion:nil];
    }
    else
    {
        CGRect rect = CGRectMake(
                                 rootViewController.view.frame.size.width * 0.5f,
                                 rootViewController.view.frame.size.height * 0.25f, 0, 0
                                 );
        
        UIPopoverController *popup = [[UIPopoverController alloc]
                                      initWithContentViewController:activity];
        
        [popup presentPopoverFromRect:rect
                               inView:rootViewController.view
             permittedArrowDirections:UIPopoverArrowDirectionAny
                             animated:YES];
    }
}
