#import <Foundation/Foundation.h>
#import "NativeCallProxy.h"


@implementation FrameworkLibAPI

id<NativeCallsProtocol> api = NULL;
+(void) registerAPIforNativeCalls:(id<NativeCallsProtocol>) aApi
{
    api = aApi;
}

@end


extern "C" {
    void showHostMainWindow(const char* color) { return [api showHostMainWindow:[NSString stringWithUTF8String:color]]; }
}
extern "C"
{
    void onViewerClicked(const char* message)
    {
        return [api onViewerClicked:[NSString stringWithUTF8String:message]];
    }
}
extern "C"
{
    void onViewerLoaded(const char* message)
    {
        return [api onViewerLoaded:[NSString stringWithUTF8String:message]];
    }
}
extern "C"
{
    void onViewerMoved(const char* message)
    {
        return [api onViewerMoved:[NSString stringWithUTF8String:message]];
    }
}



