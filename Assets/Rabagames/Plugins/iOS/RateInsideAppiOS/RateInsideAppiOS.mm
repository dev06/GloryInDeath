//
//  RateInsideAppiOS.mm
//  RateInsideAppiOS
//
//  Created by Maciej Krzykwa on 12/27/17.
//  Copyright © 2017 Raba Games. All rights reserved.
//

#import "RateInsideAppiOS.h"

@implementation RateInsideAppiOS
    + (BOOL)DisplayReviewController {
        if([SKStoreReviewController class]){
            [SKStoreReviewController requestReview] ;
            return true;
        }
        else
        {
            return false;
        }
    }
@end

extern "C"
{
    BOOL _DisplayReviewController()
    {
        return [RateInsideAppiOS DisplayReviewController];
    }
}
