'''
Brief:
    File to help get git info in a git/hg style format from the given repo

Author(s):
    Charles Machalow
'''
import subprocess
import os

def hasChanges():
    '''
    Brief:
        Returns True if changes are staged locally
    '''
    output = subprocess.check_output('git status -s -uno', shell=True)
    return bool(output.strip()) # True if output from this command
    
def getCurrentBranch():
    '''
    Brief:
        Returns the name of the current branch
    '''
    output = subprocess.check_output('git rev-parse --abbrev-ref HEAD', shell=True)
    return output.strip().decode()
    
def getListOfCommits(branch='master'):
    '''
    Brief:
        Returns list of commits on given branch. 0th is most recent.
    '''
    commits = subprocess.check_output('git log --pretty=format:%%h --full-history %s' % branch, shell=True).decode().splitlines()
    return commits
    
def getCurrentCommitId(branch='master'):
    '''
    Brief:
        Gets the hash of the current commit and appenda a "+" if there are staged changes
    '''
    commit = getListOfCommits(branch)[0] # first is newest
    if hasChanges():
        return commit + "+"
    return commit
    
def getHgStyleIdNum(branch='master'):
    '''
    Brief:
        Gets an hg-style id number for the current commit
    '''
    commits = getListOfCommits(branch)
    start = str(len(commits))
    if hasChanges():
        return start + "+"
    return start
    
def getRepoNameFromCurrentFolder():
    '''
    Brief:
        Gets the name of the repo from the current folder. Assumes the folder name was not changed
    '''
    return os.path.basename(os.getcwd())

def getOriginUrl(repoPath='.'):
    '''
    Brief:
        Gets the url of the origin.
    '''
    return subprocess.check_output('git config --get remote.origin.url', shell=True).decode().splitlines()[0]

def getRepoRevisionSetInfo(repoPath='.'):
    '''
    Brief:
        Returns a string of information about the current repository commits/changesets
    '''
    oldCwd = os.getcwd()
    os.chdir(repoPath)
    try:
        branch = getCurrentBranch()
        hgIdNum = getHgStyleIdNum(branch)
        commitId = getCurrentCommitId(branch)
        repoName = getRepoNameFromCurrentFolder()
        origin = getOriginUrl()
        return "%s - %s (hg:%s) - %s - %s" % (repoName, commitId, hgIdNum, branch, origin)
    finally:
        os.chdir(oldCwd)
        

FILE = \
'''
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cBorderless
{
    class RevisionInfo
    {
        public static string RevisionString = "<cBorderless>";
    }
}

'''
if __name__ == '__main__':
    print (getRepoRevisionSetInfo())
    txt = FILE.replace("<cBorderless>", getRepoRevisionSetInfo())
    with open('RevisionInfo.cs', 'r') as f:
        rewrite = (f.read() != txt)
    
    if rewrite:
        print ("Updating revision!")
        with open('RevisionInfo.cs', 'w') as f:
            f.write(txt)
    else:
        print ("Not Updating Revision!")