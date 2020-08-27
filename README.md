# MailDiary

is a small tool to allow writing a diary by sending mails to another mail. The programs reads out the mails
and creates chunks of Markdown files. Those can then in turn be concatenated with mdmerge or shell magic.

## Configuration example

    ---
    
    markdown-base-path: "c:/tools/maildiary"
    
    mail:
      server: "imap.some-server.de"
      port: 993
      user: super-secret-user
      password: even-more-secret-password
    
    processing:
      whitelisted-senders:
        - some@test.de
        - users@test.de
        - may@test.de
        - send@test.de
      date-time-format: "dd.MM.yyyy HH:mm:ss"
      template: |
        ## ({{received}}) {{subject}}
        
        {{content}}

The configuration sample will have MailDiary to connect to imap.some-server.de using IMAP and iterate over all mails. Foreach mail that was sent from one of the whitelisted senders it will generate a markdown file using the template provided (defaulting to the above one).

As a templating language this tool uses [scriban](https://github.com/lunet-io/scriban). For informatino about built in functions and how to use it read the documentation of scriban.

The markdown file will be named after hour, minute and second of the time when the mail was received and stored in a subfolder `year/month/day` in `markdown-base-path`.

Concatenation of generated markdown files is by design not part of this tool.

## History

|Version|Description|
|---|---|
||- assume all lowercase whiteliested emails|
|1.1.0.0|- add renderer|
||- add preserve mails flag|
||- add ability to set date time format string|
|1.0.0.0|Initial version|
