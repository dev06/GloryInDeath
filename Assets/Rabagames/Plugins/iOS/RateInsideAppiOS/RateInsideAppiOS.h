//
//  RateInsideAppiOS.h
//  RateInsideAppiOS
//
//  Created by Maciej Krzykwa on 12/27/17.
//  Copyright © 2017 Raba Games. All rights reserved.
//

#ifndef RateInsideAppiOS_h
#define RateInsideAppiOS_h

#import <Foundation/Foundation.h>
#import <StoreKit/StoreKit.h>

@interface RateInsideAppiOS : NSObject
+ (BOOL)DisplayReviewController;
@end

extern "C"
{
    BOOL _DisplayReviewController();
}

#endif /* RateInsideAppiOS_h */
